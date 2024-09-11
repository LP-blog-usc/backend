﻿namespace Blog.Models.Dtos.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
    }

}

