using System;
using System.Collections.Generic;
using System.Text;

namespace ErpSolution.Application.Responses
{
    public class ResponseText
    {
        public static string OK { get; } = "OK";

        public static string InternalServerError { get; } = "Internal Server Error";

        public static string WrongUserPassword { get; } = "NIK / NIM tidak ditemukan atau kata sandi salah";
    }
}
