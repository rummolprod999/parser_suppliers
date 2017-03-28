using System;
using System.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace parser_suppliers
{
    public class Parser
    {
        public static int log_insert = 0;
        public static int log_update = 0;
        public static int inn_null = 0;
        private readonly string supplier;

        private string inn,
            kpp,
            ogrn,
            regionCode,
            organizationName,
            postAddress,
            contactPhone,
            contactFax,
            contactEMail,
            lastName,
            middleName,
            firstName,
            contact_name = "";

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
            kpp = (string) json.SelectToken("kpp") ?? "";
            contracts223_count = (int?) json.SelectToken("contracts223Count") ?? 0;
            contracts_count = (int?) json.SelectToken("contractsCount") ?? 0;
            contracts223_sum = (double?) json.SelectToken("contracts223Sum") ?? 0.0;
            contracts_sum = (double?) json.SelectToken("contractsSum") ?? 0.0;
            ogrn = (string) json.SelectToken("ogrn") ?? "";
            regionCode = (string) json.SelectToken("regionCode") ?? "";
            organizationName = (string) json.SelectToken("organizationName") ?? "";
            postAddress = (string) json.SelectToken("postAddress") ?? "";
            contactFax = (string) json.SelectToken("contactFax") ?? "";
            contactEMail = (string) json.SelectToken("contactEMail") ?? "";
            contactPhone = (string) json.SelectToken("contactPhone") ?? "";
            middleName = (string) json.SelectToken("contactInfo.middleName") ?? "";
            firstName = (string) json.SelectToken("contactInfo.firstName") ?? "";
            lastName = (string) json.SelectToken("contactInfo.lastName") ?? "";
            contact_name = $"{firstName} {middleName} {lastName}";
//            Console.WriteLine(contact_name + "\n\n");
            inn = (string) json.SelectToken("inn") ?? "";
            if (inn != "")
            {
                MySqlConnection connect =
                    ConnectToDb.GetDBConnection("localhost", Program.Db, "test", "Dft56Point");
                connect.Open();
                if (kpp != "")
                {
                    string cmdSelect =
                        $"SELECT count(id) FROM od_supplier{Program.sfx} WHERE inn = @inn AND kpp = @kpp";
                    MySqlCommand cmd = new MySqlCommand(cmdSelect, connect);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@inn", inn);
                    cmd.Parameters.AddWithValue("@kpp", kpp);
                    int amt = 0;
                    object amtUnchecked = cmd.ExecuteScalar();
                    if (amtUnchecked != DBNull.Value && amtUnchecked != null)
                        amt = Convert.ToInt32(amtUnchecked);
                    if (amt == 0)
                    {
                        string cmdInsertWithKpp =
                            $"INSERT INTO od_supplier{Program.sfx} SET inn = @inn, kpp = @kpp, contracts_count = @contracts_count, contracts223_count = @contracts223_count, contracts_sum = @contracts_sum, contracts223_sum = @contracts223_sum, ogrn = @ogrn,region_code = @region_code, organizationName = @organizationName,postal_address = @postal_address, contactPhone = @contactPhone,contactFax = @contactFax, contactEMail = @contactEMail,contact_name = @contact_name";
                        MySqlCommand cmdInsertKpp = new MySqlCommand(cmdInsertWithKpp, connect);
                        cmdInsertKpp.Prepare();
                        cmdInsertKpp.Parameters.AddWithValue("@inn", inn);
                        cmdInsertKpp.Parameters.AddWithValue("@kpp", kpp);
                        cmdInsertKpp.Parameters.AddWithValue("@contracts_count", contracts_count);
                        cmdInsertKpp.Parameters.AddWithValue("@contracts223_count", contracts223_count);
                        cmdInsertKpp.Parameters.AddWithValue("@contracts_sum", contracts_sum);
                        cmdInsertKpp.Parameters.AddWithValue("@contracts223_sum", contracts223_sum);
                        cmdInsertKpp.Parameters.AddWithValue("@ogrn", ogrn);
                        cmdInsertKpp.Parameters.AddWithValue("@region_code", regionCode);
                        cmdInsertKpp.Parameters.AddWithValue("@organizationName", organizationName);
                        cmdInsertKpp.Parameters.AddWithValue("@postal_address", postAddress);
                        cmdInsertKpp.Parameters.AddWithValue("@contactPhone", contactPhone);
                        cmdInsertKpp.Parameters.AddWithValue("@contactFax", contactFax);
                        cmdInsertKpp.Parameters.AddWithValue("@contactEMail", contactEMail);
                        cmdInsertKpp.Parameters.AddWithValue("@contact_name", contact_name);
                        cmdInsertKpp.ExecuteNonQuery();
                        log_insert++;
                    }
                    else
                    {
                        string cmdUpdateWithKpp =
                            $"UPDATE od_supplier{Program.sfx} SET contracts_count = @contracts_count, contracts223_count = @contracts223_count, contracts_sum = @contracts_sum, contracts223_sum = @contracts223_sum, ogrn = @ogrn,region_code = @region_code, organizationName = @organizationName,postal_address = @postal_address, contactPhone = @contactPhone,contactFax = @contactFax, contactEMail = @contactEMail,contact_name = @contact_name WHERE inn = @inn AND kpp =@kpp";
                        MySqlCommand cmdUpdateKpp = new MySqlCommand(cmdUpdateWithKpp, connect);
                        cmdUpdateKpp.Prepare();
                        cmdUpdateKpp.Parameters.AddWithValue("@inn", inn);
                        cmdUpdateKpp.Parameters.AddWithValue("@kpp", kpp);
                        cmdUpdateKpp.Parameters.AddWithValue("@contracts_count", contracts_count);
                        cmdUpdateKpp.Parameters.AddWithValue("@contracts223_count", contracts223_count);
                        cmdUpdateKpp.Parameters.AddWithValue("@contracts_sum", contracts_sum);
                        cmdUpdateKpp.Parameters.AddWithValue("@contracts223_sum", contracts223_sum);
                        cmdUpdateKpp.Parameters.AddWithValue("@ogrn", ogrn);
                        cmdUpdateKpp.Parameters.AddWithValue("@region_code", regionCode);
                        cmdUpdateKpp.Parameters.AddWithValue("@organizationName", organizationName);
                        cmdUpdateKpp.Parameters.AddWithValue("@postal_address", postAddress);
                        cmdUpdateKpp.Parameters.AddWithValue("@contactPhone", contactPhone);
                        cmdUpdateKpp.Parameters.AddWithValue("@contactFax", contactFax);
                        cmdUpdateKpp.Parameters.AddWithValue("@contactEMail", contactEMail);
                        cmdUpdateKpp.Parameters.AddWithValue("@contact_name", contact_name);
                        cmdUpdateKpp.ExecuteNonQuery();
                        log_update++;
                    }
                }
                else
                {
                    string cmdSelect =
                        $"SELECT count(id) FROM od_supplier{Program.sfx} WHERE inn = @inn AND kpp = @kpp";
                    MySqlCommand cmd = new MySqlCommand(cmdSelect, connect);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@inn", inn);
                    cmd.Parameters.AddWithValue("@kpp", kpp);
                    int amt = 0;
                    object amtUnchecked = cmd.ExecuteScalar();
                    if (amtUnchecked != DBNull.Value && amtUnchecked != null)
                        amt = Convert.ToInt32(amtUnchecked);
                    if (amt == 0)
                    {
                        string cmdInsertWithoutKpp =
                            $"INSERT INTO od_supplier{Program.sfx} SET inn = @inn, kpp = @kpp, contracts_count = @contracts_count, contracts223_count = @contracts223_count, contracts_sum = @contracts_sum, contracts223_sum = @contracts223_sum, ogrn = @ogrn,region_code = @region_code, organizationName = @organizationName,postal_address = @postal_address, contactPhone = @contactPhone,contactFax = @contactFax, contactEMail = @contactEMail,contact_name = @contact_name";
                        MySqlCommand cmdInsertInn = new MySqlCommand(cmdInsertWithoutKpp, connect);
                        cmdInsertInn.Prepare();
                        cmdInsertInn.Parameters.AddWithValue("@inn", inn);
                        cmdInsertInn.Parameters.AddWithValue("@kpp", kpp);
                        cmdInsertInn.Parameters.AddWithValue("@contracts_count", contracts_count);
                        cmdInsertInn.Parameters.AddWithValue("@contracts223_count", contracts223_count);
                        cmdInsertInn.Parameters.AddWithValue("@contracts_sum", contracts_sum);
                        cmdInsertInn.Parameters.AddWithValue("@contracts223_sum", contracts223_sum);
                        cmdInsertInn.Parameters.AddWithValue("@ogrn", ogrn);
                        cmdInsertInn.Parameters.AddWithValue("@region_code", regionCode);
                        cmdInsertInn.Parameters.AddWithValue("@organizationName", organizationName);
                        cmdInsertInn.Parameters.AddWithValue("@postal_address", postAddress);
                        cmdInsertInn.Parameters.AddWithValue("@contactPhone", contactPhone);
                        cmdInsertInn.Parameters.AddWithValue("@contactFax", contactFax);
                        cmdInsertInn.Parameters.AddWithValue("@contactEMail", contactEMail);
                        cmdInsertInn.Parameters.AddWithValue("@contact_name", contact_name);
                        cmdInsertInn.ExecuteNonQuery();
                        log_insert++;
                    }
                    else
                    {
                        string cmdUpdateWithOutKpp =
                            $"UPDATE od_supplier{Program.sfx} SET contracts_count = @contracts_count, contracts223_count = @contracts223_count, contracts_sum = @contracts_sum, contracts223_sum = @contracts223_sum, ogrn = @ogrn,region_code = @region_code, organizationName = @organizationName,postal_address = @postal_address, contactPhone = @contactPhone,contactFax = @contactFax, contactEMail = @contactEMail,contact_name = @contact_name WHERE inn = @inn AND kpp =@kpp";
                        MySqlCommand cmdUpdateInn = new MySqlCommand(cmdUpdateWithOutKpp, connect);
                        cmdUpdateInn.Prepare();
                        cmdUpdateInn.Parameters.AddWithValue("@inn", inn);
                        cmdUpdateInn.Parameters.AddWithValue("@kpp", kpp);
                        cmdUpdateInn.Parameters.AddWithValue("@contracts_count", contracts_count);
                        cmdUpdateInn.Parameters.AddWithValue("@contracts223_count", contracts223_count);
                        cmdUpdateInn.Parameters.AddWithValue("@contracts_sum", contracts_sum);
                        cmdUpdateInn.Parameters.AddWithValue("@contracts223_sum", contracts223_sum);
                        cmdUpdateInn.Parameters.AddWithValue("@ogrn", ogrn);
                        cmdUpdateInn.Parameters.AddWithValue("@region_code", regionCode);
                        cmdUpdateInn.Parameters.AddWithValue("@organizationName", organizationName);
                        cmdUpdateInn.Parameters.AddWithValue("@postal_address", postAddress);
                        cmdUpdateInn.Parameters.AddWithValue("@contactPhone", contactPhone);
                        cmdUpdateInn.Parameters.AddWithValue("@contactFax", contactFax);
                        cmdUpdateInn.Parameters.AddWithValue("@contactEMail", contactEMail);
                        cmdUpdateInn.Parameters.AddWithValue("@contact_name", contact_name);
                        cmdUpdateInn.ExecuteNonQuery();
                        log_update++;
                    }
                }


                connect.Close();
                connect = null;
            }
            else
            {
                inn_null++;
            }
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