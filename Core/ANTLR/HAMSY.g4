grammar HAMSY;

program : variableDeclaration* functionDefinition* mainFunction EOF ;

functionDefinition
    : 'int' IDENTIFIER '(' 'int' IDENTIFIER ',' 'int' IDENTIFIER ')' '{' statement* returnStatement '}' // Other functions
    ;

mainFunction 
    : 'int' 'main' '(' ')' '{' statement* returnStatement '}' // Main function
    ;

returnStatement : 'return' expression ';' ;

statement
    : variableDeclaration
    | assignment
    | whileLoop
    | ifStatement
    ;

variableDeclaration : 'int' IDENTIFIER '=' expression ';' ;

assignment : IDENTIFIER '=' expression ';' ;

whileLoop : 'while' '(' condition ')' '{' statement* '}' ;

ifStatement : 'if' '(' condition ')' '{' statement* '}' ( elseStatement )? ;

elseStatement : 'else' '{' statement* '}' ;

expression 
    : operand                           
    | operand operator operand          
    | functionCall               
    ;

functionCall : IDENTIFIER '(' expression ',' expression ')' ;

operand : IDENTIFIER | INT ;

operator : '+' | '-' | '*' | '/' | '%' ;

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
COMMA: ',';

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
