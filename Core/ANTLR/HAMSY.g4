grammar HAMSY;

// Entry point
program : variableDeclaration* functionDefinition* mainFunction EOF ;

// Function Definitions
functionDefinition
    : 'int' IDENTIFIER '(' 'int' IDENTIFIER ',' 'int' IDENTIFIER ')' '{' statement* returnStatement '}' // Other functions
    ;

mainFunction 
    : 'int' 'main' '(' ')' '{' statement* returnStatement '}' // Main function
    ;

// Return Statement
returnStatement : 'return' expression ';' ;

// Statements
statement
    : variableDeclaration
    | assignment
    | whileLoop
    | ifStatement
    ;

// Variable Declarations (Global or Local)
variableDeclaration : 'int' IDENTIFIER '=' expression ';' ;

assignment : IDENTIFIER '=' expression ';' ;

// While Loop
whileLoop : 'while' '(' condition ')' '{' statement* '}' ;

// If-Else Statement
ifStatement : 'if' '(' condition ')' '{' statement* '}' ( 'else' '{' statement* '}' )? ;

// Expressions (Allows Single Operands, Binary Expressions, and Function Calls)
expression 
    : operand                           
    | operand operator operand          
    | functionCall               
    ;

// Function Calls
functionCall : IDENTIFIER '(' expression ',' expression ')' ;

operand : IDENTIFIER | INT ;

operator : '+' | '-' | '*' | '/' | '%' ;

// Conditions
condition : operand comparisonOperator operand ;

comparisonOperator : '>' | '<' | '>=' | '<=' | '==' | '!=' ;

// Tokens
INT_TYPE: 'int';
MAIN: 'main';
RETURN: 'return';
WHILE: 'while';
IF: 'if';
ELSE: 'else';

LPAREN: '(';
RPAREN: ')';
LBRACE: '{';
RBRACE: '}';
SEMI: ';';
ASSIGN: '=';

PLUS: '+';
MINUS: '-';
MUL: '*';
DIV: '/';
MOD: '%';

GT: '>';
LT: '<';
GE: '>=';
LE: '<=';
EQ: '==';
NEQ: '!=';

IDENTIFIER : [a-zA-Z_][a-zA-Z0-9_]* ;
INT : '0' | [1-9][0-9]* ;

// Ignore whitespace and comments
WS : [ \t\r\n]+ -> skip ;
COMMENT : '//' ~[\r\n]* -> skip ;
