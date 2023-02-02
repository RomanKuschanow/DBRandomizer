using System;

namespace DBRandomizer
{
    public record CollumnInfo(string Name, Type Type, bool NN, bool PK, bool AI, bool U, bool FK, string Default);
}
