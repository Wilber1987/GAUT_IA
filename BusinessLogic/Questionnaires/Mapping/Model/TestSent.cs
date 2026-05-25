using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;

namespace BusinessLogic.Questionnaires.Mapping
{
    public class TestSent: EntityClass
    {
        [PrimaryKey(Identity = true)]
        public int? IdTestSent { get; set; }
        public int? Id_test { get; set; }
        public string? Token { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public TestSentEnum? State { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? DateOfFilling { get; set; }
    }

    public enum TestSentEnum
    {
        ACTIVE, INACTIVE, FINISH, PROCESSED
    }
}