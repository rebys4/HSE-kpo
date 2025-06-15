// entity/OutboxEvent.java
package ru.hse.paymentsservice.entity;

import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDateTime;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class OutboxEvent {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String aggregateType;
    private String aggregateId;

    private String type;

    @Lob
    private String payload;

    private LocalDateTime createdAt;
    private boolean sent = false;
}