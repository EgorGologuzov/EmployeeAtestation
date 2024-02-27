using Utils;

var newData = await StoredDataUpdateUtil.UpdateStoredData();
var encryptedNewData = Encryptor.EncryptString(Encryptor.ScaleKeyTo256Bit("HLv64s"), newData);

Console.WriteLine(newData);
Console.WriteLine(encryptedNewData);
Console.ReadKey();
