using Core.Domain.Entities;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public class RegisterAllocator
    {
        private List<List<string>> _basicBlocks { get; set; } = [];
        public List<string> Output { get; set; } = [];

        private Dictionary<string, PhysicalRegister> _pRegisters { get; set; } = [];
        private HashSet<string> _free { get; set; } = [];
        private Dictionary<string, VirtualRegister> _vRegisters { get; set; } = [];

        public RegisterAllocator(List<string> assembly)
        {
            DivideIntoBlocks(assembly);
            Process();
        }

        private void DivideIntoBlocks(List<string> instructions)
        {
            foreach (string i in instructions)
            {
                string inst = i.Trim();

                if (inst.StartsWith("label") || inst.Contains(":"))
                {
                    if (_basicBlocks.Count > 0 && _basicBlocks.Last().Count == 0)
                    {
                        _basicBlocks.Last().Add(inst);
                    }
                    else
                    {
                        _basicBlocks.Add([inst]);
                    }
                }
                else if (inst.Contains("cbr"))
                {
                    _basicBlocks.Last().Add(inst);
                    _basicBlocks.Add([]);
                }
                else
                {
                    if (_basicBlocks.Count == 0)
                    {
                        _basicBlocks.Add([inst]);
                    }
                    else
                    {
                        _basicBlocks.Last().Add(inst);
                    }
                }
            }

            foreach (var bb in _basicBlocks)
            {
                Console.WriteLine("BLOCKKKKKK");
                foreach (var i in bb)
                {
                    Console.WriteLine(i);
                }
                Console.WriteLine();
            }
        }

        private void Reset(List<string> bb)
        {
            _pRegisters = [];
            _free = [];
            _vRegisters = [];

            int numberOfPRegisters = 4;
            for (int i = 0; i < numberOfPRegisters; i++) _pRegisters.Add($"p{i}", new() { ID = $"p{i}" });
            for (int i = 0; i < numberOfPRegisters; i++) _free.Add($"p{i}");
            GetVRegisters(bb, _vRegisters);
            GetUses(bb, _vRegisters);
        }

        private void Process()
        {
            foreach (List<string> bb in _basicBlocks)
            {
                Reset(bb);

                for (int i = 0; i < bb.Count; i++)
                {
                    string inst = bb[i].Trim();

                    // op op1, op2 -> dest
                    if (inst.StartsWith("cmp")
                        || inst.StartsWith("add")
                        || inst.StartsWith("sub")
                        || inst.StartsWith("div")
                        || inst.StartsWith("mult")
                    )
                    {
                        List<string> instArr = inst.Split(' ').ToList();
                        string operation = instArr[0];
                        string r1Name = instArr[1];
                        string r2Name = instArr[3];
                        string r3Name = instArr[5];

                        VirtualRegister r1 = _vRegisters[r1Name];
                        VirtualRegister r2 = _vRegisters[r2Name];
                        VirtualRegister r3 = _vRegisters[r3Name];

                        PhysicalRegister p1 = Ensure(r1);
                        PhysicalRegister p2 = Ensure(r2);

                        r1.Uses.Remove(i);
                        r2.Uses.Remove(i);

                        if (r1.Uses.Count == 0) FreeP(p1);
                        if (r2.Uses.Count == 0) FreeP(p2);

                        PhysicalRegister p3 = Allocate(r3);
                        Output.Add($"{operation} {p1.ID} , {p2.ID} -> {p3.ID}");

                        if (r1.Uses.Count != 0) UpdatePNext(p1);
                        if (r2.Uses.Count != 0) UpdatePNext(p2);
                        if (r3.Uses.Count != 0) UpdatePNext(p3);
                    }
                    // store r1 -> add
                    else if (inst.StartsWith("store"))
                    {
                        List<string> instArr = inst.Split(' ').ToList();
                        string r1Name = instArr[1];
                        string address = instArr[3];

                        VirtualRegister r1 = _vRegisters[r1Name];

                        PhysicalRegister p1 = Ensure(r1);

                        r1.Uses.Remove(i);

                        if (r1.Uses.Count == 0) FreeP(p1);

                        Output.Add($"store {p1.ID} -> {address}");

                        if (r1.Uses.Count != 0) UpdatePNext(p1);
                    }
                    // load add -> r1
                    else if (inst.StartsWith("load"))
                    {
                        List<string> instArr = inst.Split(' ').ToList();
                        string address = instArr[1];
                        string r1Name = instArr[3];

                        VirtualRegister r1 = _vRegisters[r1Name];

                        r1.Address = address;
                        PhysicalRegister p1 = Allocate(r1);

                        if (int.TryParse(address, out int _))
                        {
                            Output.Add($"loadI {address} -> {p1.ID}");
                        }
                        else
                        {
                            Output.Add($"load {address} -> {p1.ID}");
                        }

                        if (r1.Uses.Count != 0) UpdatePNext(p1);
                    }
                    // cbr r1 -> L0, L1
                    else if (inst.StartsWith("cbr"))
                    {
                        List<string> instArr = inst.Split(' ').ToList();
                        string r1Name = instArr[1];
                        string l1 = instArr[3];
                        string l2 = instArr[4];

                        VirtualRegister r1 = _vRegisters[r1Name];

                        PhysicalRegister p1 = Ensure(r1);

                        r1.Uses.Remove(i);

                        if (r1.Uses.Count == 0) FreeP(p1);

                        Output.Add($"cbr {p1.ID} -> {l1} {l2}");

                        if (r1.Uses.Count != 0) UpdatePNext(p1);
                    }
                    // push_param r8
                    else if (inst.StartsWith("push_param"))
                    {
                        List<string> instArr = inst.Split(' ').ToList();
                        string r1Name = instArr[1];

                        VirtualRegister r1 = _vRegisters[r1Name];

                        PhysicalRegister p1 = Ensure(r1);

                        r1.Uses.Remove(i);

                        if (r1.Uses.Count == 0) FreeP(p1);

                        Output.Add($"push_param {p1.ID}");

                        if (r1.Uses.Count != 0) UpdatePNext(p1);
                    }
                    // call func -> r10
                    else if (inst.StartsWith("call"))
                    {
                        List<string> instArr = inst.Split(' ').ToList();
                        string func = instArr[1];
                        string r1Name = instArr[3];

                        VirtualRegister r1 = _vRegisters[r1Name];

                        PhysicalRegister p1 = Allocate(r1);

                        Output.Add($"call {func} -> {p1.ID}");

                        if (r1.Uses.Count != 0) UpdatePNext(p1);
                    }
                    else
                    {
                        Output.Add(inst);
                    }
                }
            }
        }

        private PhysicalRegister Ensure(VirtualRegister vR)
        {
            PhysicalRegister? p = null;

            if (vR.PRegister != null) p = vR.PRegister;
            else
            {
                p = Allocate(vR);

                if (vR.InMemory)
                {
                    EmitLoad(vR, p);
                }

                vR.InMemory = false;
            }

            return p;
        }

        private PhysicalRegister Allocate(VirtualRegister vR)
        {
            PhysicalRegister? p = null;

            if (_free.Count != 0)
            {
                p = _pRegisters[_free.First()];
                _free.Remove(_free.First());
            }
            else
            {
                p = GetPWithGreatestNext();
                EmitStore(p);
                p.VRegister!.PRegister = null;
                p.VRegister.InMemory = true;
            }

            p.NextUse = -1;
            p.VRegister = vR;
            vR.PRegister = p;

            return p;
        }

        private void FreeP(PhysicalRegister p)
        {
            p.VRegister = null;
            p.NextUse = int.MaxValue;
            _free.Add(p.ID);
        }

        private void UpdatePNext(PhysicalRegister p)
        {
            p.NextUse = p.VRegister!.Uses.First();
        }

        private PhysicalRegister GetPWithGreatestNext()
        {
            int max = _pRegisters.First().Value.NextUse;
            PhysicalRegister pMax = _pRegisters.First().Value;
            foreach (var e in _pRegisters)
            {
                PhysicalRegister p = e.Value;
                if (p.NextUse > max)
                {
                    max = p.NextUse;
                    pMax = p;
                }
            }
            return pMax;
        }

        private void EmitLoad(VirtualRegister vR, PhysicalRegister pR)
        {
            if (int.TryParse(vR.Address, out int _))
            {
                Output.Add($"loadI {vR.Address} -> {pR.ID}");
            }
            else
            {
                Output.Add($"load {vR.Address} -> {pR.ID}");
            }
        }

        private void EmitStore(PhysicalRegister pR)
        {
            // TODO: HANDLE ADDRESS
            Output.Add($"store {pR.ID} -> {pR.VRegister!.Address}");
        }

        private void GetVRegisters(List<string> block, Dictionary<string, VirtualRegister> vRegisters)
        {
            Regex pattern = new(@"\br\d+\b");

            foreach (string inst in block)
            {
                var matches = pattern.Matches(inst);
                foreach (Match m in matches)
                {
                    if (!vRegisters.ContainsKey(m.Value))
                    {
                        vRegisters.Add(m.Value, new() { ID = m.Value });
                    }
                }
            }
        }

        private void GetUses(List<string> block, Dictionary<string, VirtualRegister> vRegisters)
        {
            for (int i = 0; i < block.Count; i++)
            {
                string inst = block[i];
                if (inst.Contains("->"))
                {
                    Regex pattern = new(@"\br\d+\b");
                    string left = inst.Split("->")[0];
                    var matches = pattern.Matches(left);

                    foreach (Match m in matches)
                    {
                        vRegisters[m.Value].Uses.Add(i);
                    }
                }
                else if (inst.StartsWith("push_param"))
                {
                    string r = inst.Split(" ")[1];

                    vRegisters[r].Uses.Add(i);
                }
            }
        }
    }
}