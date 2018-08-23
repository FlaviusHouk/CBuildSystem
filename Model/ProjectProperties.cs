using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CBuildSystem.Model
{
    [Serializable]
    public class ProjectProperties : IXmlSerializable
    {
        private bool _useAnsi = false;
        private CStandardEnum _standard = CStandardEnum.C11;
        private bool _useGNU89Inline = false;
        private bool _useStandardFLTEval = true;
        private string _auxInfo = string.Empty;
        private bool _IsParameterlessVariadicFunctionsAllowed = false;
        private bool _isFNOASM = false;
        private bool _isBuiltFuncIgnored = false;
        //toDo: Builtin-FuncName
        private bool _isGimpleAllowed = false;
        private bool _useExplicitEnvironent = false;
        private EnvironmentEnum _runEnvironmet = EnvironmentEnum.Hosting;
        private bool _isOpenACCUsed = false;
        private string _openACCDim = string.Empty;
        private bool _isOpenMPUsed = false;
        public ProjectProperties()
        {}
        
        public bool IsAnsi
        {
            get
            {
                return _useAnsi;
            }
            set
            {
                _useAnsi = value;
                _standard = CStandardEnum.C89;
            }
        }

        public CStandardEnum Standard 
        { 
            get => _standard; 
            set => _standard = value; 
        }

        public bool IsGNU89Inline
        {
            get
            {
                return _useGNU89Inline;
            }
            set
            {
                if(Standard == CStandardEnum.C99)
                {
                    _useGNU89Inline = value;
                }
            }
        }

        public bool UseStandardFLTEval { get => _useStandardFLTEval; set => _useStandardFLTEval = value; }
        public string AuxInfo { get => _auxInfo; set => _auxInfo = value; }
        public bool IsParameterlessVariadicFunctionsAllowed { get => _IsParameterlessVariadicFunctionsAllowed; set => _IsParameterlessVariadicFunctionsAllowed = value; }
        public bool IsFNOASM { get => _isFNOASM; set => _isFNOASM = value; }
        public bool IsBuiltFuncIgnored { get => _isBuiltFuncIgnored; set => _isBuiltFuncIgnored = value; }
        public bool IsGimpleAllowed { get => _isGimpleAllowed; set => _isGimpleAllowed = value; }
        public bool UseExplicitEnvironent { get => _useExplicitEnvironent; set => _useExplicitEnvironent = value; }
        public EnvironmentEnum RunEnvironmet { get => _runEnvironmet; set => _runEnvironmet = value; }
        public bool IsOpenACCUsed { get => _isOpenACCUsed; set => _isOpenACCUsed = value; }
        public string OpenACCDim { get => _openACCDim; set => _openACCDim = value; }
        public bool IsOpenMPUsed { get => _isOpenMPUsed; set => _isOpenMPUsed = value; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}