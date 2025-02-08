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
                    Names = s.Names,
                });
            }

            return Ok(new
            {
                SymbolTables = simplifiedSymbolTables,
                res.Notes,
                res.Errors
            });
        }
    }
}
