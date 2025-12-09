using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Features.Validator
{
    public static class ValidatorForFrm
    {
        public static bool Check(string input, string type)
        {
            switch (type.ToLower())
            {
                case "name":
                    if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
                    {
                        MessageBox.Show("Tên không hợp lệ!");
                        return false;
                    }
                    return true;

                case "phone":
                    if (!Regex.IsMatch(input, @"^[0-9]{10,11}$"))
                    {
                        MessageBox.Show("Số điện thoại không hợp lệ!");
                        return false;
                    }
                    return true;

                case "email":
                    if (!Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        MessageBox.Show("Email không hợp lệ!");
                        return false;
                    }
                    return true;
                case "passport":
                    if (!Regex.IsMatch(input, @"^[A-Za-z0-9]{6,9}$"))
                    {
                        MessageBox.Show("Số hộ chiếu không hợp lệ! (Chỉ chữ và số, 6–9 ký tự)");
                        return false;
                    }
                    return true;
                case "seat":
                    if (input==""||input==null)
                    {
                        MessageBox.Show("Vui lòng chọn ghế ngồi");
                        return false;
                    }
                    return true;
                default:
                    MessageBox.Show($"Loại kiểm tra '{type}' không hợp lệ!");
                    return false;
            }
        }
    }
}
