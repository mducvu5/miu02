﻿using NadekoBot.Common.Configs;

namespace Nadeko.Medusa;

public sealed class MedusaConfigService : ConfigServiceBase<MedusaConfig>, IMedusaConfigService
{
    private const string FILE_PATH = "medusae/medusa.yml";
    private static readonly TypedKey<MedusaConfig> _changeKey = new("config.medusa.updated");

    public override string Name
        => "medusa";

    public MedusaConfigService(
        IConfigSeria serializer,
        IPubSub pubSub)
        : base(FILE_PATH, serializer, pubSub, _changeKey)
    {   
    }

    public IReadOnlyCollection<string> GetLoadedMedusae()
        => Data.Loaded.ToList();

    public void AddLoadedMedusa(string name)
    {
        name = name.Trim().ToLowerInvariant();
        
        ModifyConfig(conf =>
        {
            if(!conf.Loaded.Contains(name))
                conf.Loaded.Add(name);
        });
    }
    
    public void RemoveLoadedMedusa(string name)
    {
        name = name.Trim().ToLowerInvariant();
        
        ModifyConfig(conf =>
        {
            conf.Loaded.Remove(name);
        });
    }
}