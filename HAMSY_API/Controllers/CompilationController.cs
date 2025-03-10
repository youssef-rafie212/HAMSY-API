﻿using Core.Domain.Entities;
using Core.DTO;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace HAMSY_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompilationController : ControllerBase
    {
        private readonly ICompilationService _compilationService;

        public CompilationController(ICompilationService compilationService)
        {
            _compilationService = compilationService;
        }

        [HttpPost("lexical-analysis")]
        public IActionResult LexicalAnalysis(LexicalRequestDto req)
        {
            LexicalResponseDto res = _compilationService.LexicalAnalysis(req);
            return Ok(res);
        }

        [HttpPost("syntax-analysis")]
        public IActionResult SyntaxAnalysis(SyntaxRequestDto req)
        {
            SyntaxResponseDto res = _compilationService.SyntaxAnalysis(req);
            return Ok(res);
        }

        [HttpPost("symbol-tables")]
        public IActionResult SymbolTables(SymbolTablesRequestDto req)
        {
            SymbolTableResponseDto res = _compilationService.SymbolTables(req);

            // Mapping SymbolTable to the simplified version.
            List<SimplifiedSymbolTable> simplifiedSymbolTables = [];
            foreach (SymbolTable s in res.SymbolTables)
            {
                simplifiedSymbolTables.Add(new()
                {
                    Scope = s.Scope,
                    Names = s.NamesTypes,
                });
            }

            return Ok(new
            {
                SymbolTables = simplifiedSymbolTables,
                res.Errors
            });
        }

        [HttpPost("semantic-analysis")]
        public IActionResult SemanticAnalysis(SemanticRequestDto req)
        {
            SemanticResponseDto res = _compilationService.SemanticAnalysis(req);
            return Ok(res);
        }

        [HttpPost("ir-generation")]
        public IActionResult IRGeneration(IRGenRequestDto req)
        {
            IRGenResponseDto res = _compilationService.IRGeneration(req);
            return Ok(res);
        }

        [HttpPost("ir-optimization")]
        public IActionResult IROptimization(IROptRequestDto req)
        {
            IROptResponseDto res = _compilationService.IROptimization(req);
            return Ok(res);
        }

        [HttpPost("instruction-selection")]
        public IActionResult InstructionSelection(InsSelRequestDto req)
        {
            InsSelResponseDto res = _compilationService.InstructionSelection(req);
            return Ok(res);
        }

        [HttpPost("register-allocation")]
        public IActionResult RegisterAllocation(RegAllocRequestDto req)
        {
            RegAllocResponseDto res = _compilationService.RegisterAllocation(req);
            return Ok(res);
        }

        [HttpPost("instruction-scheduling")]
        public IActionResult RegisterAllocation(InsSchedRequestDto req)
        {
            InsSchedResponseDto res = _compilationService.InstructionScheduling(req);
            return Ok(res);
        }
    }
}
