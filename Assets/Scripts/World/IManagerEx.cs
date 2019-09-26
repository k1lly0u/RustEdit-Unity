using System.Collections;

public interface IManagerEx
{
    void Cleanup();
    void Save(ref WorldSerialization blob);
    IEnumerator Load(World.Data world);
}
