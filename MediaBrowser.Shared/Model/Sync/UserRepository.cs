using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Phone.Toolkit.Services;
using MediaBrowser.ApiInteraction.Data;
using MediaBrowser.Model.Dto;
using MediaBrowser.WindowsPhone.Extensions;
using ScottIsAFool.WindowsPhone.Extensions;

namespace MediaBrowser.WindowsPhone.Model.Sync
{
    public class UserRepository : IUserRepository
    {
        private const string ItemsFile = "Cache\\Users.json"; 
        private readonly IAsyncStorageService _storageService;

        private List<UserDto> _items; 

        public UserRepository(IAsyncStorageService storageService)
        {
            _storageService = storageService;
        }
        public Task AddOrUpdate(string id, UserDto user)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
            var item = await Get(id);
            if (item != null)
            {
                _items.Remove(item);
                await SaveItems();
            }
        }

        public Task<UserDto> Get(string id)
        {
            return GetItemInternal(x => x.Id == id);
        }

        public async Task<List<UserDto>> GetAll()
        {
            await LoadItems();
            return _items;
        }

        private async Task LoadItems()
        {
            if (!_items.IsNullOrEmpty())
            {
                return;
            }

            var json = await _storageService.ReadStringIfFileExists(ItemsFile);
            var items = await json.DeserialiseAsync<List<UserDto>>();
            _items = items;
        }

        private async Task SaveItems()
        {
            if (_items.IsNullOrEmpty())
            {
                return;
            }

            await _storageService.CreateDirectoryIfNotThere("Cache");

            var json = await _items.SerialiseAsync();
            await _storageService.WriteAllTextAsync(ItemsFile, json);
        }

        private async Task<List<UserDto>> GetItemsInternal(Func<UserDto, bool> func)
        {
            await LoadItems();

            var items = _items.Where(func);
            return items.ToList();
        }

        private async Task<UserDto> GetItemInternal(Func<UserDto, bool> func)
        {
            var items = await GetItemsInternal(func);
            return items.FirstOrDefault();
        }
    }
}