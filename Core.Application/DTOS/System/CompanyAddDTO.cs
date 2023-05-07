using Core.Application.DTOS.System;

namespace Wizzzard.Application.DTOS
{
    public class CompanyAddDTO
    {
        public string Name { get; set; }
        public  FileDTO? Logo { get; set; }
        public string EmisNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public int SchoolTypeId { get; set; }       
  
    }
}
