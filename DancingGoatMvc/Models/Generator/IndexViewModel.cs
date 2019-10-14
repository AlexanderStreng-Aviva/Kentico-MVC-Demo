namespace DancingGoat.Models.Generator
{
    public class IndexViewModel
    {
        public bool DisplaySuccessMessage
        {
            get;
            set;
        } = false;


        public bool DisplayDevelopmentErrorMessage
        {
            get;
            set;
        } = false;


        public bool DisplayAccessDeniedError
        {
            get;
            set;
        } = false;

        public bool DisplayErrorMessage
        {
            get;
            set;
        } = false;

        public string ErrorMessage { get; set; }
    }
}