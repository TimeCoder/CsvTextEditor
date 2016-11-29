﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextEditorExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.Linq;
    using Catel;
    using ICSharpCode.AvalonEdit;

    public static class TextEditorExtensions
    {
        public static void SetCaretToSpecificLineAndColumn(this TextEditor textEditor, int lineIndex, int columnIndex, int[][] columnWidthByLine)
        {
            Argument.IsNotNull(() => textEditor);

            if (lineIndex < 0 || columnIndex < 0)
            {
                return;
            }

            var textDocument = textEditor.Document;

            var lines = textDocument.Lines;
            if (lines.Count <= lineIndex)
            {
                return;
            }

            var line = textDocument.Lines[lineIndex];
            var offset = line.Offset;
            var columnOffset = columnWidthByLine[lineIndex].Take(columnIndex).Sum();

            var maxCaretOffset = textDocument.TextLength;
            var newCaretOffset = offset + columnOffset;
            textEditor.CaretOffset = maxCaretOffset > newCaretOffset ? newCaretOffset : maxCaretOffset;
        }
    }
}