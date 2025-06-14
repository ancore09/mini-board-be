-- Migration: 001_create_board_tasks_table
-- Description: Create the board_tasks table for the kanban board

CREATE TABLE IF NOT EXISTS board_tasks (
    id BIGSERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT NOT NULL,
    tags TEXT,
    state TEXT NOT NULL,
    created_at timestamp with timezone NOT NULL,
    due_date timestamp with timezone NULL
);

-- Create index on state for better query performance
CREATE INDEX IF NOT EXISTS ix_board_tasks_state ON board_tasks (state);

-- Create index on created_at for sorting
CREATE INDEX IF NOT EXISTS ix_board_tasks_created_at ON board_tasks (created_at);