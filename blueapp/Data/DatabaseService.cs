using blueapp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace blueapp.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection db;
        
        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Log.db");
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<OperationRecord>().Wait(); // OperationRecord 테이블 생성
            db.CreateTableAsync<AppLog>().Wait(); // ErrorLog 테이블 생성
        }

        #region Record 관련 메서드
        public Task<int> AddRecordAsync(OperationRecord record)
        {
            return db.InsertAsync(record);
        }
        
        public Task<List<OperationRecord>> GetAllRecordsAsync()
        {
            //OrderByDescending(x => x.RequestTime)를 사용하여 역순으로 정렬 가능
            //return db.Table<EvRecord>().OrderByDescending(x => x.RequestTime).ToListAsync(); // 비동기적으로 모든 레코드를 가져옵니다
            return db.Table<OperationRecord>().ToListAsync(); // 비동기적으로 모든 레코드를 가져옵니다
        }
        
        public async Task<List<OperationRecord>> GetRecordsAsync(int skip, int take)
        {
            return await db.Table<OperationRecord>()
                           .OrderByDescending(x => x.Timestamp)
                           .Skip(skip)
                           .Take(take)
                           .ToListAsync();
        }
        #endregion

        #region AppLog 관련 메서드
        public Task<int> AddAppLogAsync(AppLog log)
        {
            return db.InsertAsync(log);
        }
        
        public Task<List<AppLog>> GetAllAppLogsAsync()
        {
            return db.Table<AppLog>().OrderByDescending(x => x.Timestamp).ToListAsync(); // 역순으로 정렬하여 모든 오류 로그 가져오기
        }
        
        public async Task<List<AppLog>> GetAppLogsAsync(int skip, int take)
        {
            return await db.Table<AppLog>()
                           .OrderByDescending(x => x.Timestamp)
                           .Skip(skip)
                           .Take(take)
                           .ToListAsync();
        }
        #endregion
    }
}
