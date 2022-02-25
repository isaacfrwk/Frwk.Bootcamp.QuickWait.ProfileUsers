using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Helpers;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Interfaces;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Validator;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Constants;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Entities;
using Frwk.Bootcamp.QuickWait.ProfileUsers.Domain.Interfaces;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Frwk.Bootcamp.QuickWait.ProfileUsers.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly string topicNameUser;
        private readonly string topicNameUserResponse;
        private readonly IProduceService produceService;
        private readonly UserValidator validatorValidator;
        public UserService(IUserRepository userRepository, IProduceService produceService, UserValidator validatorValidator)
        {
            this.userRepository = userRepository;
            this.topicNameUser = Topics.topicNameUser;
            this.topicNameUserResponse = Topics.topicNameUserResponse;
            this.produceService = produceService;
            this.validatorValidator = validatorValidator;
        }

        public async Task InsertAsync(User entity)
        {
            try
            {
                var validator = validatorValidator.Validate(entity);

                entity.Password = UserHelper.GenerateHashMd5(entity.Password);

                if (validator.IsValid)
                {
                    await userRepository.AddAsync(entity);
                    await userRepository.SaveChangesAsync();

                    //Chamando o producer do microsserviço auth.
                    await produceService.Call(new MessageInput(null, MethodConstant.POST, JsonConvert.SerializeObject(entity)), topicNameUser);
                    //resposta do microsserviço.
                    await produceService.Call(new MessageInput(200, MethodConstant.POST, JsonConvert.SerializeObject(entity)), topicNameUserResponse);
                }
                else
                {
                    List<string> listErro = new();

                    foreach(var error in validator.Errors)
                    {
                        listErro.Add(error.ErrorMessage);
                    }
                    await produceService.Call(new MessageInput(400, MethodConstant.POST, JsonConvert.SerializeObject(listErro)), topicNameUserResponse);
                }

            }
            catch (Exception ex)
            {
                await produceService.Call(new MessageInput(400, MethodConstant.POST, "Ocorreu um erro ao tentar salvar o usuário."), topicNameUserResponse);
            }
           
        }

        public async Task DeleteAsync(User entity)
        {
            try
            {
                userRepository.DeleteSync(entity);
                await userRepository.SaveChangesAsync();

                var message = new MessageInput(null, MethodConstant.DELETE, JsonConvert.SerializeObject(entity));

                await produceService.Call(message, topicNameUser);
            }
            catch (Exception ex)
            {
                await produceService.Call(new MessageInput(400, MethodConstant.DELETE, "erro ao tentar deletar o usuário"), topicNameUserResponse);
            }

        }

        public async Task DeleteManyAsync(IEnumerable<User> entities)
        {
            try
            {
                userRepository.DeleteManySync(entities);
                await userRepository.SaveChangesAsync();

                var message = new MessageInput(null, MethodConstant.DELETEMANY, JsonConvert.SerializeObject(entities));

                await produceService.Call(message, topicNameUser);
            }
            catch (Exception ex)
            {
                await produceService.Call(new MessageInput(400, MethodConstant.DELETE, "erro ao tentar deletar o usuário"), topicNameUserResponse);
            }

        }

        public async Task<IEnumerable<User>> FindAllAsync(bool asNoTracking = true)
        {
            var users = await userRepository.FindAllAsync(asNoTracking);

            int status = users != null ? 200: 404;

            var message = new MessageInput(status, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));

            await produceService.Call(message, topicNameUserResponse);

            return users;
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, bool asNoTracking = true)
        {
            var users = await userRepository.FindAsync(predicate, asNoTracking);

            int status = users != null ? 200 : 404;

            var message = new MessageInput(status, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));

            await produceService.Call(message, topicNameUserResponse);

            return users;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var users = await userRepository.GetByIdAsync(id);

            int status = users != null ? 200 : 404;

            var message = new MessageInput(status, MethodConstant.FINDALL, JsonConvert.SerializeObject(users));

            await produceService.Call(message, topicNameUserResponse);

            return users;
        }

        public async Task UpdateAsync(User entity)
        {
            try
            {
                var validator = validatorValidator.Validate(entity);

                if (validator.IsValid)
                {
                    await userRepository.UpdateAsync(entity);
                    await userRepository.SaveChangesAsync();

                    var message = new MessageInput(null, MethodConstant.PUT, JsonConvert.SerializeObject(entity));

                    await produceService.Call(message, topicNameUser);
                }
                else
                {
                    await produceService.Call(new MessageInput(400, MethodConstant.PUT, JsonConvert.SerializeObject(validator.Errors)), topicNameUserResponse);
                }
            }
            catch (Exception ex)
            {
                await produceService.Call(new MessageInput(400, MethodConstant.PUT, "erro ao tentar atualizar o usuário"), topicNameUserResponse);
            }

        }

    }
}
