package com.mycompany.mymeetings.modules.administration.infrastructure;

import com.mycompany.mymeetings.modules.administration.application.contracts.Command;
import com.mycompany.mymeetings.modules.administration.application.contracts.CommandHandler;
import com.mycompany.mymeetings.modules.administration.application.contracts.CommandHandlerWithResult;
import com.mycompany.mymeetings.modules.administration.application.contracts.CommandWithResult;
import com.mycompany.mymeetings.modules.administration.application.contracts.CommandsScheduler;
import org.springframework.context.ApplicationContext;
import org.springframework.core.ResolvableType;
import org.springframework.stereotype.Component;

@Component
public class InProcessCommandsScheduler implements CommandsScheduler {

    private final ApplicationContext applicationContext;

    public InProcessCommandsScheduler(ApplicationContext applicationContext) {
        this.applicationContext = applicationContext;
    }

    @Override
    @SuppressWarnings("unchecked")
    public void enqueue(Command command) {
        ResolvableType handlerType = ResolvableType.forClassWithGenerics(
                CommandHandler.class, command.getClass());
        String[] beanNames = applicationContext.getBeanNamesForType(handlerType);
        if (beanNames.length > 0) {
            CommandHandler<Command> handler = (CommandHandler<Command>) applicationContext.getBean(beanNames[0]);
            handler.handle(command);
        }
    }

    @Override
    @SuppressWarnings("unchecked")
    public <TResult> void enqueue(CommandWithResult<TResult> command) {
        ResolvableType handlerType = ResolvableType.forClassWithGenerics(
                CommandHandlerWithResult.class, command.getClass(), Object.class);
        String[] beanNames = applicationContext.getBeanNamesForType(handlerType);
        if (beanNames.length > 0) {
            CommandHandlerWithResult<CommandWithResult<TResult>, TResult> handler =
                    (CommandHandlerWithResult<CommandWithResult<TResult>, TResult>) applicationContext.getBean(beanNames[0]);
            handler.handle(command);
        }
    }
}
