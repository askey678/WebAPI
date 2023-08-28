

namespace WebApi.DAL.ExceptionHandler
{
    public class ResourceNotFoundException: Exception
    {
        public string resourcename;
        public string fieldname;
        public string fieldvalue;

        public ResourceNotFoundException() { }
        public ResourceNotFoundException(string resourcename, string fieldname, string fieldvalue)
           : base(String.Format("{0} Resource not found with {1}: {2}", resourcename, fieldname, fieldvalue))
        {
            this.resourcename = resourcename;
            this.fieldname = fieldname;  
            this.fieldvalue = fieldvalue;

        }
    }
}
