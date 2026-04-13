package com.mycompany.mymeetings.modules.administration.infrastructure;

import com.mycompany.mymeetings.buildingblocks.infrastructure.OutboxMessage;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.util.List;

@Component
public class OutboxProcessor {

    private final OutboxMessageRepository outboxMessageRepository;

    public OutboxProcessor(OutboxMessageRepository outboxMessageRepository) {
        this.outboxMessageRepository = outboxMessageRepository;
    }

    @Scheduled(fixedDelay = 2000)
    @Transactional
    public void processOutbox() {
        List<OutboxMessage> unprocessedMessages = outboxMessageRepository.findByProcessedDateIsNull();

        for (OutboxMessage message : unprocessedMessages) {
            // Process the message (e.g., publish to external bus)
            message.setProcessedDate(LocalDateTime.now());
            outboxMessageRepository.save(message);
        }
    }
}
