module WavePacker

    open System.IO
    open System.Text

    let pack (d:int16[]) =
        let stream = new MemoryStream();
        let writer = new BinaryWriter(stream, Encoding.ASCII);
        let dataLength = Array.length d * 2

        // RIFF
        writer.Write(Encoding.ASCII.GetBytes("RIFF"))
        writer.Write(dataLength+36)
        writer.Write(Encoding.ASCII.GetBytes("WAVE"))

        // fmt
        writer.Write(Encoding.ASCII.GetBytes("fmt "))
        writer.Write(16)
        writer.Write(1s)        // PCM
        writer.Write(1s)        // mono
        writer.Write(44100)     // sample rate
        writer.Write((44100 * 16) / 8)     // byte rate
        writer.Write(2s)        // bytes per sample
        writer.Write(16s)       // bits per sample

        // data
        writer.Write(Encoding.ASCII.GetBytes("data"))
        writer.Write(dataLength)
        let data:byte[] = Array.zeroCreate dataLength
        System.Buffer.BlockCopy(d, 0, data, 0, dataLength)
        writer.Write(data)
        stream

    let write fileName (stream:MemoryStream) =
        use fs = new FileStream(Path.Combine("../WaveFormWeb/wwwroot",fileName + ".wav"), FileMode.Create)
        fs |> stream.WriteTo