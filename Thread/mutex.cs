using System;
using System.Threading.Tasks;
using System.Management;
using System.Collections.Immutable;

//
// Mutex is used to protect resources on operational system level. Different 
// application podem usar o mesmo arquivo, impedir que múltiplas instâncias 
// do mesmo app rodem ao mesmo tempo, etc.
//

public class Program
{
    public static void Main()
    {

        // O nome "MeuAppUnico" identifica esse Mutex no Windows inteiro
        using (var mutex = new Mutex(false, "MeuAppUnico"))
        {
            // Tenta adquirir o acesso por 5 segundos
            if (!mutex.WaitOne(5000, false))
            {
                Console.WriteLine("Outra instância do app já está rodando!");
                return;
            }

            try 
            {
                Console.WriteLine("Rodando app...");
                Console.ReadLine();
            }
            finally
            {
                // OBRIGATÓRIO liberar, senão o Mutex fica "abandonado"
                mutex.ReleaseMutex();
            }
        }
    }
}
