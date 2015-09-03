﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Nest
{
	//TODO I Deleted DeleteExtensions, when we introduced Document as a parameter folks can do 
	//Delete(Document.Index("a").Type("x").Id("1"), s=>s)
	//Delete(Document.Infer(doc), s=>s)
	//Delete(Document.Index<T>().Type<TOptional>().Id(2), s=>s)
	//Delete(Document.Id<T>(2), s=>s)
	public partial interface IElasticClient
	{
		/// <summary>
		///The delete API allows to delete a typed JSON document from a specific index based on its id. 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/docs-delete.html
		/// </summary>
		/// <typeparam name="T">The type used to infer the default index and typename</typeparam>
		/// <param name="deleteSelector">Describe the delete operation, i.e type/index/id</param>
		IDeleteResponse Delete<T>(Func<DeleteDescriptor<T>, IDeleteRequest> deleteSelector) where T : class;

		/// <inheritdoc/>
		IDeleteResponse Delete(IDeleteRequest deleteRequest);

		/// <inheritdoc/>
		Task<IDeleteResponse> DeleteAsync<T>(Func<DeleteDescriptor<T>, IDeleteRequest> deleteSelector) where T : class;

		/// <inheritdoc/>
		Task<IDeleteResponse> DeleteAsync(IDeleteRequest deleteRequest);
	}

	public partial class ElasticClient
	{
		/// <inheritdoc/>
		public IDeleteResponse Delete<T>(Func<DeleteDescriptor<T>, IDeleteRequest> deleteSelector) where T : class =>
			this.Delete(deleteSelector?.Invoke(new DeleteDescriptor<T>()));

		/// <inheritdoc/>
		public IDeleteResponse Delete(IDeleteRequest deleteRequest) => 
			this.Dispatcher.Dispatch<IDeleteRequest, DeleteRequestParameters, DeleteResponse>(
				deleteRequest,
				(p, d) => this.LowLevelDispatch.DeleteDispatch<DeleteResponse>(p)
			);

		/// <inheritdoc/>
		public Task<IDeleteResponse> DeleteAsync<T>(Func<DeleteDescriptor<T>, IDeleteRequest> deleteSelector) where T : class => 
			this.DeleteAsync(deleteSelector?.Invoke(new DeleteDescriptor<T>()));

		/// <inheritdoc/>
		public Task<IDeleteResponse> DeleteAsync(IDeleteRequest deleteRequest) => 
			this.Dispatcher.DispatchAsync<IDeleteRequest, DeleteRequestParameters, DeleteResponse, IDeleteResponse>(
				deleteRequest,
				(p, d) => this.LowLevelDispatch.DeleteDispatchAsync<DeleteResponse>(p)
			);
	}
}