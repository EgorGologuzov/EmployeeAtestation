using Utils;

var secrets = "{\"installed\":{\"client_id\":\"310273913070-v3qbqm0epurtl57r37svqru63opor0lq.apps.googleusercontent.com\",\"project_id\":\"employeeatestation\",\"auth_uri\":\"https://accounts.google.com/o/oauth2/auth\",\"token_uri\":\"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\":\"https://www.googleapis.com/oauth2/v1/certs\",\"client_secret\":\"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\",\"redirect_uris\":[\"http://localhost\"]}}";

var newData = await StoredDataUpdateUtil.UpdateStoredData(secrets);
var encryptedNewData = Encryptor.EncryptString(Encryptor.ScaleKeyTo256Bit("HLv64s"), newData);

Console.WriteLine(newData);
Console.WriteLine(encryptedNewData);
Console.ReadKey();
