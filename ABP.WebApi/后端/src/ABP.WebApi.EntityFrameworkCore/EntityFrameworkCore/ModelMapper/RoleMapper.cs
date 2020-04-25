﻿using AutoMapper;
using System.Collections.Generic;

namespace ASF.EntityFramework.ModelMapper
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            base.CreateMap<Model.RoleModel, ASF.Domain.Entities.Role>()
              .ForMember(o => o.Permissions, model => model.MapFrom(d => new List<string>(d.Permissions.Split(',',System.StringSplitOptions.None))))
            .ForPath(a => a.CreateInfo.CreateId, model => model.MapFrom(d => d.CreateId))
                .ForPath(a => a.CreateInfo.CreateTime, model => model.MapFrom(d => d.CreateTime));

            base.CreateMap<ASF.Domain.Entities.Role, Model.RoleModel>()
              .ForMember(o => o.Permissions, e => e.MapFrom(d => string.Join(",", d.Permissions)))
               .ForPath(a => a.CreateId, model => model.MapFrom(d => d.CreateInfo.CreateId))
                .ForPath(a => a.CreateTime, model => model.MapFrom(d => d.CreateInfo.CreateTime));
        }
    }
}
