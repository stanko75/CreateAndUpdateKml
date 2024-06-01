using FunctionalTest.Log;

namespace FunctionalTest
{
    public class CancellationDecorator(ILogger logger)
    {
        public CancellationTokenSource? CancellationTokenSource;

        private readonly PostGpsPositionsFromFilesWithFileNameHandler _postGpsPositionsFromFilesWithFileNameHandler = new(logger);
        private readonly UploadImageHandler _uploadImageHandler = new(logger);
        private readonly UploadToBlogHandler _uploadToBlogHandler = new(logger);

        private CancellationToken GetCancellationToken()
        {
            CancellationTokenSource ??= new CancellationTokenSource();
            return CancellationTokenSource.Token;
        }

        private async Task ExecuteWithCancellationHandling(Func<CancellationToken, Task> action)
        {
            try
            {
                var token = GetCancellationToken();
                await action(token);
            }
            catch (OperationCanceledException)
            {
                logger.Log("Canceled.");
            }
        }

        public Task PostGpsPositionsFromFilesWithFileExecute(PostGpsPositionsFromFilesWithFileNameCommand command)
        {
            return ExecuteWithCancellationHandling(async token =>
            {
                command.CancellationToken = token;
                await _postGpsPositionsFromFilesWithFileNameHandler.Execute(command);
            });
        }

        public Task UploadImageExecute(UploadImageCommand command)
        {
            return ExecuteWithCancellationHandling(async token =>
            {
                command.CancellationToken = token;
                await _uploadImageHandler.Execute(command);
            });
        }

        public Task UploadToBlogExecute(UploadToBlogCommand command)
        {
            return ExecuteWithCancellationHandling(async token =>
            {
                command.CancellationToken = token;
                await _uploadToBlogHandler.Execute(command);
            });
        }
    }
}
