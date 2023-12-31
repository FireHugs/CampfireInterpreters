﻿namespace Campfire.TreeWalkInterpreter;

public static class Keywords
{
    public static Dictionary<string, TokenType> ReservedKeywords;

    static Keywords()
    {
        ReservedKeywords = new Dictionary<string, TokenType>
        {
            {"and", TokenType.And},
            {"class", TokenType.Class},
            {"else", TokenType.Else},
            {"false", TokenType.False},
            {"for", TokenType.For},
            {"fun", TokenType.Fun},
            {"if", TokenType.If},
            {"nil", TokenType.Nil},
            {"or", TokenType.Or},
            {"print", TokenType.Print},
            {"return", TokenType.Return},
            {"super", TokenType.Super},
            {"this", TokenType.This},
            {"true", TokenType.True},
            {"var", TokenType.Var},
            {"while", TokenType.While},
        };
    }
}
