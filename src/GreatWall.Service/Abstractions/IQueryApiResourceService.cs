﻿using GreatWall.Service.Dtos;
using GreatWall.Service.Queries;
using Util.Applications;

namespace GreatWall.Service.Abstractions {
    /// <summary>
    /// Api资源查询服务
    /// </summary>
    public interface IQueryApiResourceService : IQueryService<ApiResourceDto, ResourceQuery> {
    }
}