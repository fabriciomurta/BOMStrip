using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BOMStrip
{
    class BOMStrip
    {
        static int[] BOMSequence = { 239, 187, 191 };

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("usage: " +
                    System.AppDomain.CurrentDomain.FriendlyName +
                    " <filename>");
                return 1;
            }

            var targetFileName = args[0];
            if (File.Exists(targetFileName))
            {
                var fileHandler = File.OpenRead(targetFileName);
                Console.WriteLine("Reading '" + fileHandler.Name + "'");

                // stream to write parsed file to
                var writeBuffer = new MemoryStream(); 

                var seqPos = 0;
                var dumpCurrByte = true;

                int fileByte = fileHandler.ReadByte();

                // Writes first byte to mem stream without checking for a match
                // Allows original BOM on the file.
                if (fileByte >= 0)
                {
                    writeBuffer.WriteByte((byte)fileByte);
                }

                fileByte = fileHandler.ReadByte(); // next file byte to start
                while (fileByte >= 0)
                {
                    if (fileByte == BOMSequence[seqPos])
                    {
                        dumpCurrByte = false;
                        Console.Write("Hit @" + fileHandler.Position +
                            "! [" + fileByte + "/" + seqPos + "] ");
                        seqPos++;
                        if (seqPos >= BOMSequence.Length)
                        {
                            Console.WriteLine("Found BOM sequence!");
                            seqPos = 0;
                        }
                    }
                    else if (seqPos != 0)
                    {
                        dumpCurrByte = true;
                        Console.WriteLine("Miss @" + fileHandler.Position +
                            ". Resetting search state.");

                        Console.WriteLine("Dumping bytes up to search seq " +
                            "position " + seqPos + ".");

                        // move back the pointer to check again the byte from
                        // the beginning of the sequence
                        if (fileByte == BOMSequence[0])
                        {
                            Console.WriteLine("Sequence matches first byte " +
                                "wanted. Checking byte over from beginning.");
                            fileHandler.Seek(-1, SeekOrigin.Current);

                            // As checking again, disable dumping
                            dumpCurrByte = false;
                        }

                        // Rewind sequence position dumping matched bytes from
                        // start to end.
                        var rewCount = 0;
                        while (seqPos > 0)
                        {
                            writeBuffer.WriteByte((byte)BOMSequence[rewCount]);
                            rewCount++;
                            seqPos--;
                        }

                    }

                    if (dumpCurrByte)
                    {
                        writeBuffer.WriteByte((byte)fileByte);
                    }

                    dumpCurrByte = true; // true for next iteration

                    fileByte = fileHandler.ReadByte();
                }

                fileHandler.Close();

                // If some output, then dump it back to the file.
                if (writeBuffer.Length > 0)
                {
                    Console.Write("Saving: ");
                    var writeHandle = File.Open(targetFileName, FileMode.Create);
                    writeBuffer.WriteTo(writeHandle);
                    writeHandle.Close();
                    Console.WriteLine("done.");
                }

                writeBuffer.Close();
            }
            return 0;
        }
    }
}
