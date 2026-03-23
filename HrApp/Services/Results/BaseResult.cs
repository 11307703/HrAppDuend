namespace HrApp.Services.Results
{
    public abstract class BaseResult
    {
        private List<string> _errors = new List<string>();
        private bool _succeded;
        protected BaseResult()
        {
            _succeded = true;
        }

        public IEnumerable<string> Errors => _errors;
        public string ErrorString => string.Join(',' , _errors);
        public bool Succeeded => _succeded;
        public void Failed(string error)
        {
            _errors.Add(error);
            _succeded = false;
        }

    }
}
