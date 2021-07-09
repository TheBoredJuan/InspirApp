
[System.Serializable]
public class PatientModel
{
    public string username;
    public string password;
    public string patientName;
    public long document;
    public int gender;
    public int age;
    public int height;
    public int vitalCapacity;

    public PatientModel() { }

    public PatientModel(string username, string password, string patientName, long document, int gender, int age, int height, int vitalCapacity) {
        this.username = username;
        this.password = password;
        this.patientName = patientName;
        this.document = document;
        this.gender = gender;
        this.age = age;
        this.height = height;
        this.vitalCapacity = vitalCapacity;
    }
}
