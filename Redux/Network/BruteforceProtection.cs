
namespace Redux.Network
{
    using Redux.Utility;
    using System.Collections.Concurrent;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// This class protects the server socket system from simple brute-force attacks. It checks the IP address with a 
    /// pool of recent connections. If the IP address was found, a warning count will be incremented. If that count 
    /// reaches the maximum connections per minute limit, the socket system will block the user for a period of
    /// time defined by the server socket.
    /// </summary>
    public sealed class BruteforceProtection
    {
        // Local-Scope Variable Declarations:
        private ConcurrentDictionary<string, long> _blockedConnections; // Connections currently blocked.
        private ConcurrentDictionary<string, int> _recentConnections;   // Connections recently passed.
        private uint _maximumAttempts;  // The maximum amount of attempts before being considered as an attack.
        private uint _minutesBanned;    // The amount of minutes banned after attacking the server.

        /// <summary>
        /// This class protects the server socket system from simple brute-force attacks. It checks the IP address with a 
        /// pool of recent connections. If the IP address was found, a warning count will be incremented. If that count 
        /// reaches the maximum connections per minute limit, the socket system will block the user for a period of
        /// time defined by the server socket.
        /// </summary>
        /// <param name="maximum">The maximum amount of connections for one IP address per minute.</param>
        /// <param name="minutesBanned">The amount of minutes the user will be banned for.</param>
        public BruteforceProtection(uint maximum, uint minutesBanned)
        {
            // Initialize local-scope variables:
            _maximumAttempts = maximum;
            _minutesBanned = minutesBanned;

            // Initialize data collections / collection cleanup thread:
            _recentConnections = new ConcurrentDictionary<string, int>();
            _blockedConnections = new ConcurrentDictionary<string, long>();
            new Thread(ProtectSocket) { Priority = ThreadPriority.Lowest }.Start();
        }

        /// <summary>
        /// This method checks if an IP address has been recorded already as a problematic connection. If the connection
        /// is proven to be an attack, this method will return false for the parent method to disconnect the client.
        /// </summary>
        /// <param name="socket">The connection being authenticated.</param>
        /// <returns>True if the connection passes authentication.</returns>
        public bool Authenticate(Socket socket)
        {
            // Is the connection already blocked?
            string ipAddress = socket.RemoteEndPoint.ToString().Split(':')[0];
            if (_blockedConnections.ContainsKey(ipAddress))
                return false;

            // Alright, the connection isn't blocked. Is there a record for the connection already?
            int attempts;
            if (_recentConnections.TryGetValue(ipAddress, out attempts))
            {
                // Is the connection trying to attack the server?
                if (++_recentConnections[ipAddress] > _maximumAttempts)
                {
                    // Ban the account:
                    _blockedConnections.TryAdd(ipAddress, Common.Clock + (_minutesBanned * 60000));
                    _recentConnections.TryRemove(ipAddress, out attempts);
                    return false;
                }
            }
            else // The connection isn't even recorded. Record the new connection:
                _recentConnections.TryAdd(ipAddress, 1);
            return true;
        }

        /// <summary>
        /// This function runs on a separate thread by the class. Every minute, the thread clears the connection 
        /// pool and cleans the blocked connection pool for expired bans. If a ban has expired, it will be removed.
        /// </summary>
        private void ProtectSocket()
        {
            // Initialize variables:
            long time;

            // Run for the lifetime of the program.
            while (true)
            {
                // Sleep for a minute, then restore the connection pool.
                Thread.Sleep(60000);
                _recentConnections = new ConcurrentDictionary<string, int>();
                
                // Check blocked connections:
                foreach (string ipAddresss in _blockedConnections.Keys)
                    if (_blockedConnections[ipAddresss] < Common.Clock)
                        _blockedConnections.TryRemove(ipAddresss, out time);
            }
        }
    }
}
