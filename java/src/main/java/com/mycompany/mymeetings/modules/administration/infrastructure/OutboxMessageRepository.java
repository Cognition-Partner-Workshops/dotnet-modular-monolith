package com.mycompany.mymeetings.modules.administration.infrastructure;

import com.mycompany.mymeetings.buildingblocks.infrastructure.OutboxMessage;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.UUID;

@Repository
public interface OutboxMessageRepository extends JpaRepository<OutboxMessage, UUID> {

    List<OutboxMessage> findByProcessedDateIsNull();
}
