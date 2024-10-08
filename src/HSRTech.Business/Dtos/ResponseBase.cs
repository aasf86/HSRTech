﻿namespace HSRTech.Business.Dtos
{
    public class ResponseBase<T>
    {
        public ResponseBase(T data, Guid? requestId = null, List<string>? errors = null)
        {
            Data = data;
            RequestId = requestId.GetValueOrDefault();
            if(errors is not null) Errors.AddRange(errors);
        }

        public Guid RequestId { get; set; } = Guid.Empty;
        public T Data { get; set; } = Activator.CreateInstance<T>();
        public bool IsSuccess => !Errors.Any();
        public List<string?> Errors { get; set; } = new List<string?>();
    }

    public class ResponseBase
    {
        public static ResponseBase<T> New<T>(T data, Guid? requestId = null, List<string>? errors = null)
        {
            return new ResponseBase<T>(data, requestId, errors);
        }
    }
}
