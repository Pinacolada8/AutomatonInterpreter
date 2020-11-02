using System;

namespace App.Logging
{
    [Flags]
    public enum LogConfigEnum
    {
        CONSOLE_DEFAULT = 0b0000_0001, // 1
        CONSOLE_COLORED = 0b0000_0010, // 2
        CONSOLE_ERRORS  = 0b0000_0100, // 4
        CONSOLE_DEBUG   = 0b0000_1000, // 8
        FILE            = 0b0001_0000, // 16

        // ---
        DEFAULT        = CONSOLE_DEFAULT,                                         // 5
        IMPROVED       = CONSOLE_COLORED | CONSOLE_ERRORS                 | FILE, // 22
        DEBUG_IMPROVED = CONSOLE_COLORED | CONSOLE_ERRORS | CONSOLE_DEBUG | FILE  // 30
    }
}