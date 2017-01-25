
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace DispatchApi
{
    public partial class DispatchManager
    {
        static DispatchManager defaultInstance = new DispatchManager();
        MobileServiceClient client;

        IMobileServiceTable<images> imageTable;

        private DispatchManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

        }

        public static DispatchManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return imageTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<images>; }
        }

        public async Task<ObservableCollection<images>> GetImagesAsync(bool syncItems = false)
        {
            try
            {
                IEnumerable<images> images = await imageTable
                    .Where(image => !image.Deleted)
                    .ToEnumerableAsync();

                return new ObservableCollection<images>(images);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SaveTaskAsync(images item)
        {
            if (item.Id == null)
            {
                await imageTable.InsertAsync(item);
            }
            else
            {
                await imageTable.UpdateAsync(item);
            }
        }

        public async Task SaveTaskAsync(IEnumerable<images> items)
        {
            if (items != null && items.Count() > 0)
            {
                foreach (var i in items)
                {
                    if (i.Id == null)
                    {
                        await imageTable.InsertAsync(i);
                    }
                    else
                    {
                        await imageTable.UpdateAsync(i);
                    }
                }
            }
        }

#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.todoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allTodoItems",
                    this.todoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
    }
}
