using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;
using APPCORE.Security;
using BusinessLogic.Questionnaires.Mapping;

namespace BusinessLogic.Questionnaires.Transactional
{
    public class TestSentOperations
    {
        public static ResponseService SaveTets(TestSent testSent)
		{
            //TODO AGREGAR LISTA DE CONTACTOS
            if (testSent.PhoneNumber == null && testSent.Email == null)
            {
               return new ResponseService(500, "Datos de contacto requerido");
            }
            var profile = new Tbl_Profile().Find<Tbl_Profile>(
                //FilterData.Or(
                    FilterData.Equal("Email", testSent.Email)
                    //FilterData.Equal("PhoneNumber", testSent.PhoneNumber)
                //)
            );
            if (profile == null)
            {
                new Tbl_Profile
                {
                    Nombres = testSent.Email,
                    Correo_institucional = testSent.Email,
                }.Save();
            }
            testSent.ShippingDate = DateTime.Now;
            testSent.State = TestSentEnum.ACTIVE;
            testSent.Token = Guid.NewGuid().ToString();
            testSent.Save();
			return new ResponseService(200, "Mensaje enviado");
		}
    }
}