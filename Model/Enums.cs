using System;

namespace CBuildSystem.Model
{
    public enum CStandardEnum
    {
        C89,
        C90,
        GNU_C90,
        C99,
        GNU_C99,
        C11,
        GNU_C11,
        C17,
        GNU_C17
    }

    public enum EnvironmentEnum
    {
        Hosting,
        FreeStanding
    }
}