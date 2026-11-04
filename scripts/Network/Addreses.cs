using System.Linq;
using Godot;

public partial class Addreses: Node {
    public override void _Ready(){

    }
    public static string getCurrentIp(){
        string[] addresses = IP.GetLocalAddresses();
        string preferredIp = addresses.FirstOrDefault(ip =>
                    ip.Contains(".") &&
                    !ip.StartsWith("127.") &&
                    !ip.StartsWith("169.254")
                );

        return preferredIp ?? "127.0.0.1";
    }
}
