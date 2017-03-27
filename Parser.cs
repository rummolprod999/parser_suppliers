using System;
using Newtonsoft.Json.Linq;

namespace parser_suppliers
{
    public class Parser
    {
        private string supplier;
        private string inn, kpp, ogrn, regionCode, organizationName, postAddress, contactPhone, contactFax,
            contactEMail, lastName, middleName, firstName, contact_name = "";

        private int contracts_count, contracts223_count = 0;
        private double contracts_sum, contracts223_sum = 0.0;

        public Parser(string line)
        {
            supplier = line;
        }

        void parser_sup(string l)
        {
            var s = clear_s(l);
            JObject json = JObject.Parse(s);
            kpp = (string)json["kpp"];
            /*contracts223_count = (int)json["contracts223Count"];
            contracts_count = (int)json["contractsCount"];
            contracts223_sum = (double) json["contracts223Sum"];
            contracts_sum = (double) json["contractsSum"];
            ogrn = (string)json["ogrn"];
            regionCode = (string)json["regionCode"];
            organizationName = (string)json["organizationName"];
            postAddress = (string)json["postAddress"];
            contactFax = (string)json["contactFax"];
            contactEMail = (string)json["contactEMail"];
            contactPhone = (string)json["contactPhone"];*/
            middleName = (string) json["contactInfo"]["middleName"];
            firstName = (string) json["contactInfo"]["firstName"];
            lastName = (string) json["contactInfo"]["lastName"];
            contact_name = $"{firstName} {middleName} {lastName}";
            Console.WriteLine(contact_name + "\n\n");

        }

        public void pars()
        {
            parser_sup(supplier);
        }

        private string clear_s(string s)
        {
            string st = s;
            st = st.Trim();
            if (st.StartsWith("["))
            {
                st = st.Remove(0, 1);
            }
            if (st.IndexOf(',', (st.Length - 1)) != -1)
            {
                st = st.Remove(st.Length - 1);
            }
            if (st.IndexOf(']', (st.Length - 1)) != -1)
            {
                st = st.Remove(st.Length - 1);
            }

            return st;
        }
    }
}