using Core.DTO;
using Core.ServiceContracts;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Core.Services
{
    public class AIOptimizationService : IAIOptimizationService
    {
        public async Task<SourceOptResponseDto> Optimize(SourceOptRequestDto sourceOptRequestDto, string apiKey)
        {
            string optimizedCode = await SendRequest(sourceOptRequestDto.SourceCode, apiKey);
            return new SourceOptResponseDto { OptimizedCode = optimizedCode };
        }

        private async Task<string> SendRequest(string sourceCode, string apiKey)
        {
            string endpoint = "https://api.openai.com/v1/chat/completions";
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = """
You are a code optimizer for a custom programming language.
Only use the following grammar rules and syntax:

<program> ::= <variableDeclaration>* <functionDefinition>* <mainFunction> EOF

<functionDefinition> ::= "int" <IDENTIFIER> "(" "int" <IDENTIFIER> "," "int" <IDENTIFIER> ")" "{" <statement>* <returnStatement> "}"

<mainFunction> ::= "int" "main" "(" ")" "{" <statement>* <returnStatement> "}"

<returnStatement> ::= "return" <expression> ";"

<statement> ::= <variableDeclaration> 
              | <assignment> 
              | <whileLoop> 
              | <ifStatement>

<variableDeclaration> ::= "int" <IDENTIFIER> "=" <expression> ";"

<assignment> ::= <IDENTIFIER> "=" <expression> ";"

<whileLoop> ::= "while" "(" <condition> ")" "{" <statement>* "}"

<ifStatement> ::= "if" "(" <condition> ")" "{" <statement>* "}" <elseStatement>?

<elseStatement> ::= "else" "{" <statement>* "}"

<expression> ::= <operand>
              | <operand> <operator> <operand>
              | <functionCall>

<functionCall> ::= <IDENTIFIER> "(" <expression> "," <expression> ")"

<operand> ::= <IDENTIFIER> | <INT>

<operator> ::= "+" | "-" | "*" | "/" | "%"

<condition> ::= <operand> <comparisonOperator> <operand>

<comparisonOperator> ::= ">" | "<" | ">=" | "<=" | "==" | "!="

(* Terminals *)
<IDENTIFIER> ::= [a-zA-Z_][a-zA-Z0-9_]*
<INT> ::= "0" | [1-9][0-9]*

Do not use any constructs not mentioned in the grammar.
Return only valid optimized code that adheres strictly to this language.
Do not explain anything. Only output the optimized code.
"""
                    },
                    new
                    {
                        role = "user",
                        content = sourceCode
                    }
                },
                temperature = 0.3
            };

            string jsonRequest = JsonSerializer.Serialize(requestBody);
            using StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            return doc.RootElement.GetProperty("choices")[0]
                                  .GetProperty("message")
                                  .GetProperty("content")
                                  .GetString()?.Trim() ?? "";
        }
    }
}
