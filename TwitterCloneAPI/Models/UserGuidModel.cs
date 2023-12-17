using System.ComponentModel.DataAnnotations;

namespace TwitterCloneAPI.Models
{
    public class UserGuidModel
    {
        //[RegularExpression("^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$")]
        public string Guid { get; set; }
    }
}
