using System;

namespace DTO.CabinClass
{
    public class CabinClassDTO
    {
        #region Private Fields
        private int _classId;
        private string _className;
        private string _description;
        #endregion

        #region Public Properties
        public int ClassId
        {
            get => _classId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("ID hạng ghế không thể âm");
                _classId = value;
            }
        }

        public string ClassName
        {
            get => _className;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên hạng ghế không được để trống");

                if (value.Length > 50)
                    throw new ArgumentException("Tên hạng ghế không được quá 50 ký tự");

                _className = value.Trim();
            }
        }

        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
        #endregion

        #region Constructors
        public CabinClassDTO() { }

        public CabinClassDTO(string className, string description)
        {
            ClassName = className;
            Description = description;
        }

        public CabinClassDTO(int classId, string className, string description)
        {
            ClassId = classId;
            ClassName = className;
            Description = description;
        }
        #endregion

        #region Validation Methods
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(_className))
            {
                errorMessage = "Tên hạng ghế không được để trống";
                return false;
            }

            if (_className.Length > 50)
            {
                errorMessage = "Tên hạng ghế không được quá 50 ký tự";
                return false;
            }

            return true;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return $"{_className}" + (!string.IsNullOrEmpty(_description) ? $" - {_description}" : "");
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (CabinClassDTO)obj;
            return _classId == other._classId;
        }

        public override int GetHashCode()
        {
            return _classId.GetHashCode();
        }
        #endregion

        #region Helper Methods
        public CabinClassDTO Clone()
        {
            return new CabinClassDTO(
                _classId,
                _className,
                _description
            );
        }
        #endregion
    }
}