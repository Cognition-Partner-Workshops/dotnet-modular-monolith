package com.mycompany.mymeetings.modules.administration.infrastructure;

import com.mycompany.mymeetings.modules.administration.application.contracts.AdministrationModule;
import com.mycompany.mymeetings.modules.administration.application.contracts.Command;
import com.mycompany.mymeetings.modules.administration.application.contracts.CommandHandler;
import com.mycompany.mymeetings.modules.administration.application.contracts.CommandHandlerWithResult;
import com.mycompany.mymeetings.modules.administration.application.contracts.CommandWithResult;
import com.mycompany.mymeetings.modules.administration.application.contracts.Query;
import com.mycompany.mymeetings.modules.administration.application.contracts.QueryHandler;
import org.springframework.context.ApplicationContext;
import org.springframework.core.ResolvableType;
import org.springframework.stereotype.Service;

@Service
public class AdministrationModuleImpl implements AdministrationModule {

    private final ApplicationContext applicationContext;

    public AdministrationModuleImpl(ApplicationContext applicationContext) {
        this.applicationContext = applicationContext;
    }

    @Override
    @SuppressWarnings("unchecked")
    public void executeCommand(Command command) {
        ResolvableType handlerType = ResolvableType.forClassWithGenerics(
                CommandHandler.class, command.getClass());
        String[] beanNames = applicationContext.getBeanNamesForType(handlerType);
        if (beanNames.length == 0) {
            throw new IllegalArgumentException("No handler found for command: " + command.getClass().getName());
        }
        CommandHandler<Command> handler = (CommandHandler<Command>) applicationContext.getBean(beanNames[0]);
        handler.handle(command);
    }

    @Override
    @SuppressWarnings("unchecked")
    public <TResult> TResult executeCommand(CommandWithResult<TResult> command) {
        ResolvableType handlerType = ResolvableType.forClassWithGenerics(
                CommandHandlerWithResult.class, command.getClass(), Object.class);
        String[] beanNames = applicationContext.getBeanNamesForType(handlerType);
        if (beanNames.length == 0) {
            throw new IllegalArgumentException("No handler found for command: " + command.getClass().getName());
        }
        CommandHandlerWithResult<CommandWithResult<TResult>, TResult> handler =
                (CommandHandlerWithResult<CommandWithResult<TResult>, TResult>) applicationContext.getBean(beanNames[0]);
        return handler.handle(command);
    }

    @Override
    @SuppressWarnings("unchecked")
    public <TResult> TResult executeQuery(Query<TResult> query) {
        ResolvableType handlerType = ResolvableType.forClassWithGenerics(
                QueryHandler.class, query.getClass(), Object.class);
        String[] beanNames = applicationContext.getBeanNamesForType(handlerType);
        if (beanNames.length == 0) {
            throw new IllegalArgumentException("No handler found for query: " + query.getClass().getName());
        }
        QueryHandler<Query<TResult>, TResult> handler =
                (QueryHandler<Query<TResult>, TResult>) applicationContext.getBean(beanNames[0]);
        return handler.handle(query);
    }
}
