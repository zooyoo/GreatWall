﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreatWall.Data.Pos;
using GreatWall.Data.Stores.Abstractions;
using GreatWall.Domain.Enums;
using GreatWall.Service.Abstractions;
using GreatWall.Service.Dtos;
using GreatWall.Service.Dtos.Extensions;
using GreatWall.Service.Queries;
using Util;
using Util.Applications;
using Util.Datas.Queries;
using Util.Domains.Repositories;

namespace GreatWall.Service.Implements {
    /// <summary>
    /// Api资源查询服务
    /// </summary>
    public class QueryApiResourceService : QueryServiceBase<ResourcePo, ApiResourceDto, ResourceQuery>, IQueryApiResourceService {
        /// <summary>
        /// 初始化Api资源服务
        /// </summary>
        /// <param name="resourceStore">资源存储器</param>
        public QueryApiResourceService( IResourcePoStore resourceStore )
            : base( resourceStore ) {
            ResourceStore = resourceStore;
        }
        
        /// <summary>
        /// 资源存储器
        /// </summary>
        public IResourcePoStore ResourceStore { get; set; }

        /// <summary>
        /// 转成Api资源参数
        /// </summary>
        protected override ApiResourceDto ToDto( ResourcePo po ) {
            return po.ToApiResourceDto();
        }

        /// <summary>
        /// 创建查询对象
        /// </summary>
        /// <param name="param">查询参数</param>
        protected override IQueryBase<ResourcePo> CreateQuery( ResourceQuery param ) {
            return new Query<ResourcePo>( param )
                .Where( t => t.Type == ResourceType.Api )
                .WhereIfNotEmpty( t => t.Uri.Contains( param.Uri ) )
                .WhereIfNotEmpty( t => t.Name.Contains( param.Name ) );
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        public override async Task<List<ApiResourceDto>> GetAllAsync() {
            var entities = await ResourceStore.FindAllAsync( t => t.Type == ResourceType.Api );
            return entities.Select( ToDto ).ToList();
        }

        /// <summary>
        /// 获取资源列表
        /// </summary>
        /// <param name="uri">资源标识列表</param>
        public async Task<List<ApiResourceDto>> GetResources( List<string> uri ) {
            if( uri == null || uri.Count == 0 )
                return new List<ApiResourceDto>();
            var resources = await ResourceStore.FindAllAsync( t => uri.Contains( t.Uri ) && t.Type == ResourceType.Api );
            if( resources == null )
                return new List<ApiResourceDto>();
            return resources.Select( t => t.ToApiResourceDto() ).ToList();
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="uri">资源标识</param>
        public async Task<ApiResourceDto> GetResource( string uri ) {
            if ( uri.IsEmpty() )
                return null;
            var resource = await ResourceStore.SingleAsync( t => t.Uri == uri );
            return resource.ToApiResourceDto();
        }
    }
}