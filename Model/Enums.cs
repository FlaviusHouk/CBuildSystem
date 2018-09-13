using System;

namespace CBuildSystem.Model
{
    public enum GCCPropertiesEnum
    {
        UseAnsiProperty,
        StandardProperty,
        GNU89InlineProperty,
        StandardFLTEvalProperty,
        AuxInfoProperty,
        ParameterlessVariadicFunctionsAllowedProperty,
        FNO_ASMProperty,
        BuiltFuncIgnoredProperty,
        GimpleProperty,
        RunEnvironmetProperty,
        OpenACCProperty,
        OpenACCDimProperty,
        OpenMPProperty,
        OpenMPSimdProperty,
        GNU_TMProperty,
        MSExtensionsProperty,
        Plan9ExtensionsProperty,
        CondMismatchProperty,
        FlaxVectorConversationProperty,
        CharAlwaysSignedProperty,
        CharAlwaysUnSignedProperty,
        EndianProperty


        
    }

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

    public enum EndianEnum
    {
        BigEndian,
        LittleEndian,
        Native
    }
}