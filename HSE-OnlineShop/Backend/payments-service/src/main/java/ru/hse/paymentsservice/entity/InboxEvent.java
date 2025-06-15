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
public class InboxEvent {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String eventId;
    private String aggregateType;
    private String type;
    @Lob
    private String payload;

    private LocalDateTime receivedAt;
    private boolean processed = false;
}