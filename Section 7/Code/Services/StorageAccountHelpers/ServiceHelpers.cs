using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacktCourse.AppDemo.Services.StorageAccountHelpers
{
    // Do not use in production code, consider Key Vault or other secure services.
    public class ServiceHelpers
    {
        public static string Key1 => "M9T3dvZrUPPEesTiPpoDG+PqpE9spBNT1rUlOLylzKKdenKigIjfJ5ctqqNQbckLpN98v5VdzCxlKoLSxEq2KA==";
        public static string ConnectionStringKey1 => "DefaultEndpointsProtocol=https;AccountName=packtcoursestorage;AccountKey=M9T3dvZrUPPEesTiPpoDG+PqpE9spBNT1rUlOLylzKKdenKigIjfJ5ctqqNQbckLpN98v5VdzCxlKoLSxEq2KA==;EndpointSuffix=core.windows.net";

        public static string Key2 => "d9DSOuBX3YFbTwlS5hplbPO1KLgs86WUu7tDz3scMCFg+FqaiEpTZtzEun4Tj1ocr+7m7yrt+jBpPwKEoan7xw==";
        public static string ConnectionStringKey2 => "DefaultEndpointsProtocol=https;AccountName=packtcoursestorage;AccountKey=d9DSOuBX3YFbTwlS5hplbPO1KLgs86WUu7tDz3scMCFg+FqaiEpTZtzEun4Tj1ocr+7m7yrt+jBpPwKEoan7xw==;EndpointSuffix=core.windows.net";
    }
}
