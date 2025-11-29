using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Role {
    public class RoleDTO {
        private int _id;
        private string _code = "";
        private string _name = "";
        public int RoleId {
            get => _id;
            set {
                if(value < 0)
                    throw new ArgumentOutOfRangeException("RoleId không hợp lệ");
                _id = value;
            }
        }
        public string RoleCode {
            get => _code;
            set {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã vai trò rỗng");
                if(value.Length > 50)
                    throw new Exception("Mã vai trò quá dài (<=50)");
                _code = value.Trim();
            }
        }
        public string RoleName {
            get => _name;
            set {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên vai trò rỗng");
                if(value.Length >100)
                    throw new Exception("Tên vai trò quá dài (<=100)");
                _name = value.Trim();
            }
        }
    }
}
